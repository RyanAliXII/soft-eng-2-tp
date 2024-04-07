import { ref } from "vue";
import { AnyZodObject, ZodError, ZodIssue } from "zod";
type UseFormProps<T extends Object> = {
  initialValue: T;
  schema?: AnyZodObject;
};

export const useForm = <T extends Object>({
  initialValue,
  schema,
}: UseFormProps<T>) => {
  const form = ref<T>(initialValue);
  const errors = ref({});
  const parseAndValidate = (): T | undefined => {
    try {
      if (!schema) return undefined;
      const parsedForm = schema?.parse(form.value) as T;
      return parsedForm;
    } catch (err) {
      const errorCollection = {};
      if (err instanceof ZodError) {
        err.errors.forEach((issue) => {
          traverseError(issue, 0, errorCollection);
        });
        errors.value = errorCollection;
      }
      return undefined;
    }
  };
  return {
    form,
    errors,
    parseAndValidate,
  };
};

const traverseError = (
  issue: ZodIssue,
  depth = 0,
  errorObj: Record<string | number, any> = {}
) => {
  const key = issue.path[depth];
  const value = errorObj[key];
  if (depth + 1 === issue.path.length) {
    if (value) {
      errorObj[key].push(issue.message);
    } else {
      errorObj[key] = [issue.message];
    }
    return;
  }
  if (value) {
    traverseError(issue, depth + 1, value);
  } else {
    errorObj[key] = {};
    traverseError(issue, depth + 1, errorObj[key]);
  }
};

import { get, has, set, unset } from "lodash";
import { ref, onUnmounted } from "vue";
import { AnyObject, InferType, ObjectSchema, ValidationError } from "yup";
type UseFormProps<T extends Object, S extends AnyObject> = {
  initialValue: T;
  schema?: ObjectSchema<S>;
};
type InputElements = HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement;

type ErrorType = Record<number | string, string[]>;
export const useForm = <T extends Object, S extends AnyObject>({
  initialValue,
  schema,
}: UseFormProps<T, S>) => {
  const form = ref<T>(initialValue);
  const errors = ref<ErrorType>({});
  const inputs = ref<InputElements[]>([]);
  const parse = (): InferType<typeof schema> | undefined => {
    try {
      const d = schema.cast(form.value);
      return d;
    } catch (err) {
      console.error(err);
      return undefined;
    }
  };
  const registerInput = (el: InputElements) => {
    if (!el) {
      return;
    }
    if (inputs.value.includes(el)) {
      return;
    }
    el.addEventListener("blur", blurHandler);
    if (el.tagName === "select") {
      el.addEventListener("change", inputHandler);
      return;
    }
    el.addEventListener("input", inputHandler);
    inputs.value.push(el);
  };
  const inputHandler = (event: Event) => {
    const target = event.target as InputElements;
    const name = target.name ?? "";
    const value = target.value ?? "";
    if (!name) {
      console.warn("input element has no name attribute");
      return;
    }
    set(form.value as object, name, value);
  };
  const blurHandler = (event: Event) => {
    const target = event.target as InputElements;
    const name = target.name;
    if (has(errors.value, name)) {
      unset(errors.value, name);
    }
  };
  onUnmounted(() => {
    inputs.value.forEach((input) => {
      input.removeEventListener("blur", blurHandler);
      input.removeEventListener("input", inputHandler);
    });
  });
  const parseAndValidate = async (): Promise<
    InferType<typeof schema> | undefined
  > => {
    try {
      const d = await schema.validate(form.value, { abortEarly: false });
      return d;
    } catch (err) {
      if (err instanceof ValidationError) {
        let errorObject: any = {};
        err.inner.forEach((err) => {
          const field = get(errorObject, err.path);
          if (field) {
            field.push(err.message);
          } else {
            set(errorObject, err.path, err.errors);
          }
        });
      }
      return undefined;
    }
  };
  const setErrors = (inputErrors: ErrorType) => {
    let errorObject: ErrorType = {};
    for (const [key, value] of Object.entries(inputErrors)) {
      set(errorObject, key, value);
    }
    errors.value = errorObject;
  };

  return {
    form,
    errors,
    parse,
    parseAndValidate,
    registerInput,
    setErrors,
  };
};

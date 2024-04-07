import { createApp } from "vue";
import { useForm } from "./composables/useForm";
import { object, string, coerce } from "zod";

type User = {
  name: string;
  age: number;
  account: {
    email: string;
  };
};
createApp({
  setup() {
    const { form, parseAndValidate, errors } = useForm<User>({
      initialValue: {
        name: "",
        age: 0,
        account: {
          email: "",
        },
      },
      schema: object({
        name: string(),
        age: coerce.number().min(10),
        account: object({
          email: string().email(),
        }),
      }),
    });

    const onSubmit = () => {
      const f = parseAndValidate();
    };

    return {
      form,
      onSubmit,
      errors,
    };
  },
}).mount("#eventPage");

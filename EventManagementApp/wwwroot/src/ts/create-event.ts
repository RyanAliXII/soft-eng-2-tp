import { createApp } from "vue";
import { useForm } from "./composables/useForm";
import { Event } from "./types/event";
import { object, string } from "zod";
import { isValidDatetime, toISO8601DateString } from "./utils/datetime";
type CreateEventForm = Omit<Event, "id">;
createApp({
  setup() {
    const { form, errors } = useForm<CreateEventForm>({
      initialValue: {
        name: "",
        activities: [],
        date: new Date(),
      },
      schema: object({
        name: string(),
      }),
    });
    const handleDateInput = (event: InputEvent) => {
      const target = event.target as HTMLInputElement;
      const value = target.value;
      try {
        const date = new Date(value);
        form.value.date = date;
      } catch (e) {
        console.error(e);
      }
    };

    return {
      form,
      errors,
      toISO8601DateString,
      handleDateInput,
    };
  },
}).mount("#createEventPage");

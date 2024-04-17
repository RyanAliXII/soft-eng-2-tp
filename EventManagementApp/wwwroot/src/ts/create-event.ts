import { createApp } from "vue";
import { useForm } from "./composables/useForm";
import { NewEvent } from "./types/event";
import { object, string } from "zod";
import { toISO8601DateString } from "./utils/datetime";

createApp({
  setup() {
    const { form, errors } = useForm<NewEvent>({
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
    const addActivity = () => {
      form.value.activities.push({
        endTime: new Date(),
        startTime: new Date(),
        name: "",
      });
    };
    const removeActivity = (rowIndex: number) => {
      console.log(rowIndex);
      form.value.activities = form.value.activities.filter(
        (a, idx) => idx != rowIndex
      );
    };

    return {
      form,
      errors,
      toISO8601DateString,
      handleDateInput,
      addActivity,
      removeActivity,
    };
  },
}).mount("#createEventPage");

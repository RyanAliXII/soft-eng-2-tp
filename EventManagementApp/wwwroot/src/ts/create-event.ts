import { StatusCodes } from "http-status-codes";
import { createApp, onMounted, ref } from "vue";
import { array, date, InferType, number, object, string } from "yup";
import { getCSRF } from "./utils/csrf";
import { get24HRTime, toISO8601DateString } from "./utils/datetime";
import { toStructuredErrors } from "./utils/form";
import VueDatePicker from "@vuepic/vue-datepicker";
import "@vuepic/vue-datepicker/dist/main.css";

createApp({
  components: {
    "date-picker": VueDatePicker,
  },
  setup() {
    const csrf = ref("");
    const currentDate = new Date();
    onMounted(() => {
      csrf.value = getCSRF();
    });

    const timeSchema = object({
      hours: number().integer(),
      minutes: number().integer(),
      seconds: number().integer(),
    });
    const activitiesSchema = array(
      object({
        name: string(),
        startTime: timeSchema,
        endTime: timeSchema,
      })
    );
    const schema = object({
      name: string().min(10, "Name is required.").email().trim(),
      date: date(),
      activities: activitiesSchema,
    });
    const INITIAL_FORM: InferType<typeof schema> = {
      name: "",
      date: new Date(),
      activities: [],
    };
    const form = ref({ ...INITIAL_FORM });
    const errors = ref({});

    const handleDateInput = (event: InputEvent) => {
      const target = event.target as HTMLInputElement;
      const value = target.value;
      try {
        const date = new Date(value);
        form.value.date = date;
        console.log(date);
      } catch (e) {
        console.error(e);
      }
    };
    const addActivity = () => {
      form.value.activities.push({
        startTime: {
          hours: 6,
          minutes: 0,
          seconds: 0,
        },
        endTime: {
          hours: 6,
          minutes: 0,
          seconds: 0,
        },
        name: "",
      });
    };
    const removeActivity = (rowIndex: number) => {
      form.value.activities = form.value.activities.filter(
        (a, idx) => idx != rowIndex
      );
    };
    const parseActivitiesTime = (
      activities: InferType<typeof activitiesSchema>
    ) => {
      const newActivities = activities.map((a) => {
        const activity = {
          name: a.name,
          startTime: get24HRTime(
            new Date(
              currentDate.getFullYear(),
              currentDate.getMonth(),
              currentDate.getDay(),
              a.startTime.hours,
              a.startTime.minutes,
              a.startTime.seconds
            )
          ),
          endTime: get24HRTime(
            new Date(
              currentDate.getFullYear(),
              currentDate.getMonth(),
              currentDate.getDay(),
              a.endTime.hours,
              a.endTime.minutes,
              a.endTime.seconds
            )
          ),
        };

        return activity;
      });
      return newActivities;
    };
    const submit = async () => {
      errors.value = {};
      const f = schema.cast(form.value);
      const activities = parseActivitiesTime(f.activities);
      const response = await fetch("/Admin/Event/Create", {
        method: "POST",
        body: JSON.stringify({
          ...f,
          date: toISO8601DateString(f.date),
          activities: activities,
        }),

        headers: new Headers({
          "Content-Type": "application/json",
          RequestVerificationToken: csrf.value,
        }),
      });

      if (response.status === StatusCodes.BAD_REQUEST) {
        const data = await response.json();
        errors.value = toStructuredErrors(data?.errors) ?? {};
        return;
      }
      if (response.status === StatusCodes.OK) {
        alert("New event has been added.");
      }
    };
    return {
      form,
      toISO8601DateString,
      handleDateInput,
      addActivity,
      removeActivity,
      submit,
      errors,
    };
  },
}).mount("#createEventPage");

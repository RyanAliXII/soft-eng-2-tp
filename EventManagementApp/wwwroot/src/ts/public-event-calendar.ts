import {
  CalendarOptions,
  EventInput,
  EventSourceFunc,
} from "@fullcalendar/core";
import dayGridPlugin from "@fullcalendar/daygrid";
import interactionPlugin from "@fullcalendar/interaction";
import timeGridPlugin from "@fullcalendar/timegrid";
import FullCalendar from "@fullcalendar/vue3";
import { createApp, ref } from "vue";
import { Event } from "./types/event";
import {
  timeToObject,
  toISO8601DateString,
  toISO8601DatetimeString,
} from "./utils/datetime";

createApp({
  components: {
    "full-calendar": FullCalendar,
  },
  setup() {
    const fullCalendar = ref<InstanceType<typeof FullCalendar> | null>(null);
    const eventUrl = new URL(window.location.origin + "/events");

    const fetchEvents: EventSourceFunc = async (info, success) => {
      const start = toISO8601DateString(info.start);
      const end = toISO8601DateString(info.end);
      eventUrl.searchParams.set("start", start);
      eventUrl.searchParams.set("end", end);
      const response = await fetch(eventUrl.toString(), {
        headers: new Headers({ "Content-Type": "application/json" }),
      });
      const data = await response.json();
      const events = (data?.events ?? []) as Event[];
      const dEvents: EventInput[] = [];

      events.forEach((e) => {
        const eventDate = new Date(e.date);
        dEvents.push({
          id: e.id,
          date: toISO8601DateString(eventDate),
          title: e.name,
          className: "event",
          extendedProps: {
            ...e,
            isActivity: false,
          },
        });
        e.activities.forEach((a) => {
          if (
            typeof a.startTime === "string" &&
            typeof a.endTime === "string"
          ) {
            const start = timeToObject(a.startTime);
            const end = timeToObject(a.endTime);
            const activityStartDatetime = toISO8601DatetimeString(
              new Date(
                eventDate.getFullYear(),
                eventDate.getMonth(),
                eventDate.getDate(),
                start.hours,
                start.minutes,
                start.seconds
              )
            );
            const activityEndDatetime = toISO8601DatetimeString(
              new Date(
                eventDate.getFullYear(),
                eventDate.getMonth(),
                eventDate.getDate(),
                end.hours,
                end.minutes,
                end.seconds
              )
            );

            dEvents.push({
              title: a.name,
              display: "test",
              start: activityStartDatetime,
              end: activityEndDatetime,
              className: "bg-success",
              extendedProps: {
                ...a,
                isActivity: true,
              },
            });
          }
        });
      });
      success(dEvents);
    };

    const calendarOptions: CalendarOptions = {
      plugins: [dayGridPlugin, interactionPlugin, timeGridPlugin],
      initialView: "dayGridMonth",
      events: fetchEvents,
      selectable: true,

      eventClick: (e) => {
        fullCalendar.value.getApi().changeView("timeGridDay", e.event.start);
      },
      headerToolbar: {
        left: "prev,next",
        center: "title",
        right: "dayGridMonth,timeGridWeek", // user can switch between the two
      },
    };

    return {
      calendarOptions,
      fullCalendar,
    };
  },
}).mount("#eventCalendar");

import { computed, createApp, onMounted, ref } from "vue";
import FullCalendar from "@fullcalendar/vue3";
import dayGridPlugin from "@fullcalendar/daygrid";
import interactionPlugin from "@fullcalendar/interaction";
import {
  CalendarOptions,
  EventInput,
  EventSourceFunc,
} from "@fullcalendar/core";
import {
  timeToObject,
  toISO8601DateString,
  toISO8601DatetimeString,
} from "./utils/datetime";
import timeGridPlugin from "@fullcalendar/timegrid";
import { Event } from "./types/event";
import { StatusCodes } from "http-status-codes";
import { getCSRF } from "./utils/csrf";
import Toast from "./toast/toast";

createApp({
  components: {
    "full-calendar": FullCalendar,
  },
  setup() {
    const fullCalendar = ref<InstanceType<typeof FullCalendar> | null>(null);
    const eventUrl = new URL(window.location.origin + "/Admin/Event");
    const eventId = ref("");
    const isEditDisable = computed(() => eventId.value.length === 0);
    const csrf = ref("");
    const toast = new Toast({ duration: 3000 });
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
                eventDate.getDay(),
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
              className: "activity",
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
    onMounted(() => {
      csrf.value = getCSRF();
    });
    const deleteEvent = async () => {
      if (eventId.value.length === 0) return;
      const response = await fetch(`/Admin/Event/Delete/${eventId.value}`, {
        method: "DELETE",
        headers: new Headers({
          RequestVerificationToken: csrf.value,
        }),
      });

      if (response.status === StatusCodes.OK) {
        toast.success(`Event has been deleted.`);
        fullCalendar.value.getApi().changeView("dayGridMonth");
        fullCalendar.value.getApi().refetchEvents();
      }
    };
    const calendarOptions: CalendarOptions = {
      plugins: [dayGridPlugin, interactionPlugin, timeGridPlugin],
      initialView: "dayGridMonth",
      events: fetchEvents,
      selectable: true,
      dateClick: (info) => {
        window.location.href = `/Admin/Event/Create?date=${info.dateStr}`;
      },
      datesSet: (info) => {
        if (info.view.type != "timeGridDay") {
          eventId.value = "";
        }
      },
      eventClick: (e) => {
        fullCalendar.value.getApi().changeView("timeGridDay", e.event.start);
        const prop = e.event.extendedProps;
        if (prop.isActivity) {
          eventId.value = prop.eventId;
        } else {
          eventId.value = prop.id;
        }
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
      eventId,
      isEditDisable,
      deleteEvent,
    };
  },
}).mount("#eventCalendar");

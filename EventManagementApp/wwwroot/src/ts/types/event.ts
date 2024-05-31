import { Activity } from "./activity";

export type Event = {
  id: string;
  name: string;
  date: Date | string;
  activities: Activity[];
};
export type NewEvent = {
  name: string;
  date: Date | string;
  activities: Omit<Activity, "id">[];
};

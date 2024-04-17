import { Activity } from "./activity";

export type Event = {
  id: string;
  name: string;
  date: Date;
  activities: Activity[];
};
export type NewEvent = {
  name: string;
  date: Date;
  activities: Omit<Activity, "id">[];
};

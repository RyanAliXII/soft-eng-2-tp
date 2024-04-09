import { Activity } from "./activity";

export type Event = {
  id: string;
  name: string;
  date: Date;
  activities: Activity[];
};

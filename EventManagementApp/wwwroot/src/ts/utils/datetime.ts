import { format } from "date-fns";

export const toISO8601DateString = (date: Date) => {
  if (!isValidDatetime(date)) return "";
  try {
    return format(date, "yyyy-MM-dd");
  } catch (error) {
    return "";
  }
};
export const toISO8601DatetimeString = (date: Date) => {
  if (!isValidDatetime(date)) return "";
  try {
    return format(date, "yyyy-MM-dd HH:mm:ss");
  } catch (error) {
    return "";
  }
};
export const get24HRTime = (date: Date) => {
  if (!isValidDatetime(date)) return "";
  try {
    return format(date, "HH:mm:ss");
  } catch (error) {
    return "";
  }
};
export const timeToObject = (timeStr: string) => {
  const [hours, minutes, seconds] = timeStr.split(":").map(Number);
  return {
    hours,
    minutes,
    seconds,
  };
};
export const isValidDatetime = (date: Date) => {
  if (date instanceof Date && !isNaN(date.valueOf())) {
    return true;
  }
  return false;
};
export const to24HRTimeString = (date: Date) => {
  if (!isValidDatetime(date)) return "";
  return date.toLocaleString(undefined, {
    hour12: false,
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit",
  });
};

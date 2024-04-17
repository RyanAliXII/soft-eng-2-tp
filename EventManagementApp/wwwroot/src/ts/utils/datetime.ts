export const toISO8601DateString = (date: Date) => {
  if (!isValidDatetime(date)) return "";
  return new Date(date).toLocaleString("sv-SE", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
  });
};
export const isValidDatetime = (date: Date) => {
  if (date instanceof Date && !isNaN(date.valueOf())) {
    return true;
  }
  return false;
};

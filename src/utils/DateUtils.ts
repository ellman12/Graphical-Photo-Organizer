export function toLocalISOString(date = new Date()) {
    const pad = (n: number) => String(n).padStart(2, "0");

    const year = date.getFullYear();
    const month = pad(date.getMonth() + 1);
    const day = pad(date.getDate());
    const hours = pad(date.getHours());
    const minutes = pad(date.getMinutes());
    const seconds = pad(date.getSeconds());
    const ms = String(date.getMilliseconds()).padStart(3, "0");

    //Include timezone offset (e.g. +09:00 or -04:00)
    // const offset = -date.getTimezoneOffset();
    // const sign = offset >= 0 ? "+" : "-";
    // const offsetHours = pad(Math.floor(Math.abs(offset) / 60));
    // const offsetMinutes = pad(Math.abs(offset) % 60);

    // return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}.${ms}${sign}${offsetHours}:${offsetMinutes}`;
    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}.${ms}Z`;
}

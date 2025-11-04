import path from "path";
import { supportedVideoExtensions } from "../electron/api";
import { exifToolService } from "../services/ExifToolService";
import { DateTaken } from "./DateTaken";

export type UnsortedFile = {
    readonly filePath: string;
    readonly dateTaken: DateTaken;
    readonly isVideo: boolean;
};

export async function createUnsortedFile(filePath: string): Promise<UnsortedFile> {
    const dateTaken = await exifToolService.getDateTaken(filePath);
    console.log(`Date taken for ${filePath}`, dateTaken);

    return {
        filePath,
        dateTaken,
        isVideo: supportedVideoExtensions.has(path.extname(filePath).toLowerCase()),
    };
}

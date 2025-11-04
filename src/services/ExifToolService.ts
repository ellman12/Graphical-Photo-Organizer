import { app } from "electron";
import { ExifTool, Tags, WriteTaskResult } from "exiftool-vendored";
import fs from "fs";
import { fileURLToPath } from "node:url";
import path from "path";
import { DateTaken, MetadataDateTakenValue } from "../types/DateTaken";

const metadataTagNames: (keyof Tags)[] = ["CreateDate", "CreationDate", "Date", "DateTime", "DateTimeCreated", "DateTimeDigitized", "DateTimeOriginal", "DigitalCreationDateTime", "MediaCreateDate", "MediaModifyDate"] as const;

export type ExifToolService = {
    exifTool: ExifTool;
    getDateTaken: (filePath: string) => Promise<DateTaken>;
    getMetadataDateTaken: (filePath: string) => Promise<MetadataDateTakenValue[]>;
    getFilenameDateTaken: (filePath: string) => Date | null;
    writeDateTaken: (filePath: string, newDateTaken: Date | string | null) => Promise<WriteTaskResult>;
};

function getExifToolPath() {
    if (app.isPackaged) {
        return path.join(process.resourcesPath, "ExifTool", "exiftool.exe");
    } else {
        return path.join(path.dirname(fileURLToPath(import.meta.url)), "../bin/ExifTool/exiftool.exe");
    }
}

//Wrapper around ExifTool for getting and setting file date taken values.
export function createExifToolService(): ExifToolService {
    const exePath = getExifToolPath();

    if (!fs.existsSync(exePath)) {
        throw new Error(`ExifTool executable not found at: ${exePath}`);
    }

    const exifTool = new ExifTool({
        exiftoolPath: exePath,
        writeArgs: ["-overwrite_original"],
    });

    async function getDateTaken(filePath: string): Promise<DateTaken> {
        const metadata = await getMetadataDateTaken(filePath);
        const filename = getFilenameDateTaken(filePath);
        return {
            metadata,
            filename,
        };
    }

    async function getMetadataDateTaken(filePath: string): Promise<MetadataDateTakenValue[]> {
        const tags = await exifTool.read(filePath);

        return metadataTagNames
            .map((key) => ({ source: key, value: tags[key] }))
            .map((field) => {
                const date = new Date(field.value?.toString() ?? "");
                const value = isNaN(date.getTime()) ? null : date;
                return { source: field.source, value };
            })
            .filter((field) => field.value !== null) as MetadataDateTakenValue[]; //as is to remove null
    }

    function getFilenameDateTaken(filePath: string): Date | null {
        //The first part handles optional junk data like "IMG_". The pattern matches filenames like: "IMG_20210320_175909.jpg", "Capture 2020-12-26 21_03_05.png", "2020-10-06_13.53.33.png"
        const PATTERN = /(\d+[-_.: ])?(\d{4})[-_.: ]?(\d{2})[-_.: ]?(\d{2})[-_.: ]?(\d{2})[-_.: ]?(\d{2})[-_.: ]?(\d{2})/;

        const filename = path.basename(filePath);
        const match = filename.match(PATTERN);
        if (!match) {
            return null;
        }

        //match[0] = entire match
        //match[1] = possible junk prefix (ignored)
        //match[2] = year, match[3] = month, ..., match[7] = seconds
        const [, , year, month, day, hour, minute, second] = match;

        const date = new Date(`${year}-${month}-${day}T${hour}:${minute}:${second}`);

        //Validate that the date is real (e.g., not "2020-13-99").
        if (isNaN(date.getTime())) {
            return null;
        }

        return date;
    }

    async function writeDateTaken(filePath: string, newDateTaken: Date | string | null): Promise<WriteTaskResult> {
        newDateTaken = newDateTaken?.toString() ?? "";
        const metadataTags = Object.fromEntries(metadataTagNames.map((key) => [key, newDateTaken]));
        return await exifTool.write(filePath, metadataTags);
    }

    return {
        exifTool,
        getDateTaken,
        getMetadataDateTaken,
        getFilenameDateTaken,
        writeDateTaken,
    };
}

export const exifToolService = createExifToolService();

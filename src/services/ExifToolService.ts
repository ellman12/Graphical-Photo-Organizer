import { app } from "electron";
import { ExifTool, WriteTaskResult } from "exiftool-vendored";
import fs from "fs";
import path from "path";
import { DateTaken, MetadataDateTakenValue, MetadataTagNames } from "../types/DateTaken";

export type ExifToolService = {
    exifTool: ExifTool;
    getExifToolPath: () => string;
    getDateTaken: (filePath: string) => Promise<DateTaken>;
    getMetadataDateTaken: (filePath: string) => Promise<MetadataDateTakenValue[]>;
    getFilenameDateTaken: (filePath: string) => Date | null;
    writeDateTaken: (filePath: string, newDateTaken: Date | string | null) => Promise<WriteTaskResult>;
};

//Wrapper around ExifTool for getting and setting file date taken values.
export function createExifToolService(): ExifToolService {
    function getExifToolPath(): string {
        if (app?.isPackaged) {
            return path.join(process.resourcesPath, "ExifTool", "exiftool.exe").replaceAll("\\", "/");
        } else {
            return path.join(process.cwd(), "bin/ExifTool/exiftool.exe").replaceAll("\\", "/");
        }
    }

    const exePath = getExifToolPath();

    if (!fs.existsSync(exePath)) {
        throw new Error(`ExifTool.exe not found at: ${exePath}`);
    } else {
        console.log(`Found ExifTool.exe at ${exePath}`);
    }

    const exifTool = new ExifTool({
        exiftoolPath: exePath,
        keepUTCTime: false,
        defaultVideosToUTC: false,
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

        return MetadataTagNames.map((key) => ({ source: key, value: tags[key] }))
            .map((field) => {
                const date = new Date(field.value?.toLocaleString() ?? "");
                const value = isNaN(date.getTime()) ? null : date;

                if (value != null) console.log(`Found metadata value for ${field.source}`, value);

                return { source: field.source, value };
            })
            .filter((field) => field.value !== null) as MetadataDateTakenValue[];
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
        const metadataTags = Object.fromEntries(MetadataTagNames.map((key) => [key, newDateTaken]));
        return await exifTool.write(filePath, metadataTags);
    }

    return {
        exifTool,
        getExifToolPath,
        getDateTaken,
        getMetadataDateTaken,
        getFilenameDateTaken,
        writeDateTaken,
    };
}

export const exifToolService = createExifToolService();

import { exiftool } from "exiftool-vendored";
import * as path from "path";
import { supportedVideoExtensions } from "../electron/api";

export type UnsortedFile = {
    readonly filePath: string;
    readonly metadataDateTaken: Date | null;
    readonly filenameDateTaken: Date | null;
    readonly isVideo: boolean;
};

export async function createUnsortedFile(filePath: string): Promise<UnsortedFile> {
    const tags = await exiftool.read(filePath);
    console.log("Creating UnsortedFile for", filePath, tags);

    return {
        filePath: filePath.replaceAll("\\", "/"),
        metadataDateTaken: null,
        filenameDateTaken: null,
        isVideo: supportedVideoExtensions.has(path.extname(filePath)),
    };
}

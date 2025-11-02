import path from "path";
import { supportedVideoExtensions } from "../electron/api";
import { exifTool } from "../services/ExifTool";

export type UnsortedFile = {
    readonly filePath: string;
    readonly metadataDateTaken: Date | null;
    readonly filenameDateTaken: Date | null;
    readonly isVideo: boolean;
};

export async function createUnsortedFile(filePath: string): Promise<UnsortedFile> {
    const tags = await exifTool.read(filePath);
    console.log("Creating UnsortedFile for", filePath, tags);

    return {
        filePath: filePath.replaceAll("\\", "/"),
        metadataDateTaken: null,
        filenameDateTaken: null,
        isVideo: supportedVideoExtensions.has(path.extname(filePath)),
    };
}

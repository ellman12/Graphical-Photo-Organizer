import fs from "fs";
import path from "path";
import { expect } from "vitest";
import { exifToolService } from "../../src/services/ExifToolService";
import { toLocalISOString } from "../../src/utils/DateUtils";
import { TestFiles } from "../TestFiles/TestFiles";

describe("ExifToolService Unit", () => {
    const testFilesRoot = path.join(process.cwd(), "tests/TestFiles");
    const updatedDateTaken = new Date().toISOString();

    it("should get ExifTool path properly", () => {
        const path = exifToolService.getExifToolPath();
        console.log(path);
        expect(path.endsWith("Graphical-Photo-Organizer/bin/ExifTool/exiftool.exe")).toBeTruthy();
    });

    test.concurrent.each(TestFiles)("Get DT for $filename", async (file) => {
        const filePath = path.join(testFilesRoot, file.filename);
        const dateTaken = await exifToolService.getDateTaken(filePath);
        console.log(file, dateTaken);

        expect(file.filenameDateTaken).toEqual(dateTaken.filename ? toLocalISOString(dateTaken.filename) : null);

        if (file.metadataDateTaken === null) {
            expect(dateTaken.metadata).toHaveLength(0);
        } else {
            const found = dateTaken.metadata.filter((m) => {
                const formatted = toLocalISOString(m.value);
                console.log(formatted);
                return formatted === file.metadataDateTaken;
            });

            console.log("Found Metadata DT:", found);
            expect(found.length).toBeGreaterThanOrEqual(1);
        }
    });

    test.concurrent.each(TestFiles)("Update DT for $filename", async (file) => {
        const sourcePath = path.join(testFilesRoot, file.filename);
        const newFolderPath = path.join(testFilesRoot, "TempFiles/Update DT");
        const newPath = path.join(newFolderPath, file.filename);

        console.log(`Copying ${sourcePath} to ${newPath}`);
        fs.mkdirSync(newFolderPath, { recursive: true });
        fs.copyFileSync(sourcePath, newPath);

        expect(fs.existsSync(newPath)).toBeTruthy();

        await exifToolService.writeDateTaken(newPath, updatedDateTaken);
        let foundMetadata = await exifToolService.getMetadataDateTaken(newPath);
        expect(foundMetadata.length).toBeGreaterThanOrEqual(1);
        expect(foundMetadata.some((m) => m.value.toISOString() === updatedDateTaken));

        await exifToolService.writeDateTaken(newPath, null);
        foundMetadata = await exifToolService.getMetadataDateTaken(newPath);
        expect(foundMetadata).toHaveLength(0);

        fs.rmSync(newPath);
    });
});

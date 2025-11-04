import { Tags } from "exiftool-vendored";

//A date taken extracted from metadata with ExifTool.
export type MetadataDateTakenValue = {
    source: keyof Tags;
    value: Date;
};

export type DateTaken = {
    //Date taken value(s) obtained from the metadata of the file, if present.
    metadata: MetadataDateTakenValue[];

    //A date taken value obtained from the name of the file, if present.
    filename: Date | null;
};

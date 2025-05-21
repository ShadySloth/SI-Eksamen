export interface Picture {
  id?: string;
  fileName: string;
  fileBase64: string;
}

export interface Label {
  id?: string;
  name: string;
  images?: Picture[];
}

export interface Segmentation {
  id?: string;
  firstCoordinateX: number;
  firstCoordinateY: number;
  secondCoordinateX: number;
  secondCoordinateY: number;
  labelId: string;
  imageId: string;
}

export interface Dataset {
  id?: string;
  dataSetName: string;
}

export interface CreateDataSet {
  dataSet: Dataset;
  labelsToBeUsed: string[];
}
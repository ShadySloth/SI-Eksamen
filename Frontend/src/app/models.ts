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

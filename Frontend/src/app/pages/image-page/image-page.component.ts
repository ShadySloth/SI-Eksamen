import {Component, ElementRef, NgIterable, OnInit, ViewChild} from '@angular/core';
import {Label, Picture, Segmentation} from "../../models";
import {ImageService} from "../../services/image.service";
import {LabelService} from "../../services/label.service";
import {SegmentationService} from "../../services/segmentation.service";

@Component({
  selector: 'app-image-page',
  templateUrl: './image-page.component.html',
  styleUrls: ['./image-page.component.css']
})

export class ImagePageComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  @ViewChild('imageRef', { static: false }) imageRef!: ElementRef<HTMLImageElement>;
  selectedImage: Picture | null = null;
  imageList: Picture[] = [];
  selectedFile: File | null = null;
  labelList: Label[] = [];
  selectedLabels: Label[] = [];
  labelForSegmentation: Label | null = null;

  drawing = false;
  startX = 0;
  startY = 0;

  finalCoords: { x1: number, y1: number, x2: number, y2: number } | null = null;

  rect = {
    left: 0,
    top: 0,
    width: 0,
    height: 0
  };

  newLabel: string = '';

  constructor(private imageService: ImageService,
              private labelService: LabelService,
              private segmentationService: SegmentationService) {}

  ngOnInit(): void {
    this.fetchImages();
    this.fetchLabels();
    }

  triggerFileSelect(): void {
    this.fileInput.nativeElement.click();  // Opens the file picker
  }

  async onFileSelected(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      await this.uploadFile();
    }
  }

  async uploadFile(): Promise<void> {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('file', this.selectedFile, this.selectedFile.name);
    var response = await this.imageService.uploadImage(formData);
    this.selectedImage = response;
    this.imageList = [...this.imageList, response];
  }

  private async fetchImages(): Promise<void> {
    // Fetch images from the backend and update imageList
    // This is a placeholder, implement the actual fetch logic
    this.imageList = await this.imageService.getImages();
    if (this.imageList.length > 0) {
      this.selectImage(this.imageList[0]);
    }
  }

  selectImage(img: Picture) {
    this.selectedImage = img;
    this.selectedLabels = this.labelList.filter(label => label.images?.some(image =>
      image.id === img.id));
    console.log("Labels for selected image: ", this.selectedLabels);
  }

  async addLabel() {
    if (this.newLabel.trim() === '') return;
    const newLabelObj: Label = {
      name: this.newLabel
    };
    var newLabel = await this.labelService.addLabel(newLabelObj);
    this.labelList = [... this.labelList, newLabel];
  }

  private async fetchLabels() {
    var labels = await this.labelService.getLabels();
    this.labelList = labels;
  }

  onLabelToggle(label: Label, event: Event): void {
    const input = event.target as HTMLInputElement;
    const checked = input.checked;

    if (checked) {
      label.images?.push(this.selectedImage!);
      this.selectedLabels.push(label);
      this.labelService.updateLabel(label);
    } else {
      this.selectedLabels = this.selectedLabels.filter(l => l.id !== label.id);
    }
  }

  isLabelSelected(label: Label): boolean {
    return this.selectedLabels.some(l => l.id === label.id);
  }


  startDraw(event: MouseEvent) {
    const bounds = this.imageRef.nativeElement.getBoundingClientRect();
    this.startX = event.clientX - bounds.left;
    this.startY = event.clientY - bounds.top;
    this.rect = { left: this.startX, top: this.startY, width: 0, height: 0 };
    this.drawing = true;
  }

  onDraw(event: MouseEvent) {
    if (!this.drawing) return;

    const bounds = this.imageRef.nativeElement.getBoundingClientRect();
    const currentX = event.clientX - bounds.left;
    const currentY = event.clientY - bounds.top;

    this.rect = {
      left: Math.min(this.startX, currentX),
      top: Math.min(this.startY, currentY),
      width: Math.abs(currentX - this.startX),
      height: Math.abs(currentY - this.startY)
    };
  }

  endDraw() {
    if (!this.drawing) return;

    this.drawing = false;

    const x1 = this.startX;
    const y1 = this.startY;
    const x2 = this.rect.left + this.rect.width;
    const y2 = this.rect.top + this.rect.height;

    this.finalCoords = { x1, y1, x2, y2 };
    this.saveCoordinates(this.finalCoords);

    console.log('Saved pixel coordinates:', this.finalCoords);
  }

  private saveCoordinates(finalCoords: { x1: number; y1: number; x2: number; y2: number }) {
    const segmentationData: Segmentation = {
      firstCoordinateX: finalCoords.x1,
      firstCoordinateY: finalCoords.y1,
      secondCoordinateX: finalCoords.x2,
      secondCoordinateY: finalCoords.y2,
      labelId: this.labelForSegmentation!.id!,
      imageId: this.selectedImage!.id!
    }

    this.segmentationService.addSegmentation(segmentationData);
    console.log("Segmentation data saved:", segmentationData);
  }
}

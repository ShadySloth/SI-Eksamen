import {Component, ElementRef, NgIterable, OnInit, ViewChild} from '@angular/core';
import {Label, PagedResult, Picture, Segmentation} from "../../models";
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
  pagedImages: PagedResult<Picture> | null = null;
  selectedFile: File | null = null;
  labelList: Label[] = [];
  selectedLabels: Label[] = [];
  activeLabel: Label | null = null;
  segmentations: Segmentation[] = [];

  deleteMode = false;
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
  totalPages: number = 0;

  constructor(private imageService: ImageService,
              private labelService: LabelService,
              private segmentationService: SegmentationService) {}

  ngOnInit(): void {
    this.fetchImages();
    this.fetchLabels();
    }

  triggerFileSelect(): void {
    this.fileInput.nativeElement.click();
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
    if (this.pagedImages){
      this.pagedImages.items = [...this.pagedImages.items, response];
      this.pagedImages.totalCount += 1;
    }
  }

  private async fetchImages(page: number = 1, pageSize: number = 10): Promise<void> {
    this.pagedImages = await this.imageService.getImages(page, pageSize);

    if (this.pagedImages?.items?.length > 0) {
      this.selectImage(this.pagedImages.items[0]);
      this.activeLabel = this.labelList[0];
      this.loadSegmentations();
    }

    this.totalPages = Math.ceil((this.pagedImages?.totalCount || 0) / (this.pagedImages?.pageSize || 1));
  }

  async loadSegmentations() {
    if (!this.selectedImage || !this.activeLabel) return;

    this.segmentations = await this.segmentationService.getSegmentationsForImageAndLabel(
      this.selectedImage.id!,
      this.activeLabel.id!
      );
  }

  selectImage(img: Picture) {
    this.selectedImage = img;

    this.selectedLabels = this.labelList.filter(label =>
      label.images?.some(image => image.id === img.id)
    );

    this.activeLabel = this.selectedLabels[0] || null;
    this.loadSegmentations();
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

  async onLabelToggle(label: Label, event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const checked = input.checked;

    const originalImages = [...(label.images ?? [])];
    const originalSelectedLabels = [...this.selectedLabels];

    try {
      if (checked) {
        label.images?.push(this.selectedImage!);
        this.selectedLabels.push(label);
      } else {
        label.images = label.images?.filter(img => img.id !== this.selectedImage?.id);
        this.selectedLabels = this.selectedLabels.filter(l => l.id !== label.id);
      }

      await this.labelService.updateLabel(label);
    } catch (error) {
      console.error('Error updating label:', error);

      label.images = originalImages;
      this.selectedLabels = originalSelectedLabels;

      input.checked = !checked;
    }
  }

  isLabelSelected(label: Label): boolean {
    return this.selectedLabels.some(l => l.id === label.id);
  }


  onPickRightLabel(label: Label) {
    this.activeLabel = label;
    this.loadSegmentations();
  }


  startDraw(event: MouseEvent) {
    if (this.deleteMode) return;
    if (!this.activeLabel) return;

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
  }

  private async saveCoordinates(finalCoords: { x1: number; y1: number; x2: number; y2: number }) {
    const segmentationData: Segmentation = {
      firstCoordinateX: finalCoords.x1,
      firstCoordinateY: finalCoords.y1,
      secondCoordinateX: finalCoords.x2,
      secondCoordinateY: finalCoords.y2,
      labelId: this.activeLabel!.id!,
      imageId: this.selectedImage!.id!
    }

    var newSegmentation = await this.segmentationService.addSegmentation(segmentationData);
    this.segmentations = [...this.segmentations, newSegmentation];
  }

  async removeSegmentation(segmentation: Segmentation, event: MouseEvent): Promise<void> {
    if (!this.deleteMode) return;
    event.stopPropagation();

    const confirmDelete = confirm('Remove this segmentation?');
    if (!confirmDelete) return;

    try {
      await this.segmentationService.deleteSegmentation(segmentation.id!);
      this.segmentations = this.segmentations.filter(s => s.id !== segmentation.id);
    } catch (err) {
      console.error('Failed to remove segmentation:', err);
    }
  }

  goToPreviousPage() {
    if (this.pagedImages && this.pagedImages.page > 1) {
      this.pagedImages.page -= 1;
      this.fetchImages(this.pagedImages.page, this.pagedImages.pageSize);
    }
  }

  goToNextPage() {
    if (this.pagedImages && this.pagedImages.page < this.totalPages) {
      this.pagedImages.page += 1;
      this.fetchImages(this.pagedImages.page, this.pagedImages.pageSize);
    }
  }

  toggleDeleteMode() {
    this.deleteMode = !this.deleteMode;
  }
}

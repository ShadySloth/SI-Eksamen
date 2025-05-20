import {Component, ElementRef, NgIterable, OnInit, ViewChild} from '@angular/core';
import {Label, Picture} from "../../models";
import {ImageService} from "../../services/image.service";
import {LabelService} from "../../services/label.service";

@Component({
  selector: 'app-image-page',
  templateUrl: './image-page.component.html',
  styleUrls: ['./image-page.component.css']
})
export class ImagePageComponent implements OnInit {
  selectedImage: Picture | null = null;
  imageList: Picture[] = [];
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  selectedFile: File | null = null;
  labelList: Label[] = [];
  selectedLabels: Label[] = [];
  newLabel: string = '';

  constructor(private imageService: ImageService,
              private labelService: LabelService) {}

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
}

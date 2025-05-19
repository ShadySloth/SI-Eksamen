import {Component, ElementRef, NgIterable, ViewChild} from '@angular/core';
import {picture} from "../../models";
import {BackendServiceService} from "../../services/backend-service.service";

@Component({
  selector: 'app-image-page',
  templateUrl: './image-page.component.html',
  styleUrls: ['./image-page.component.css']
})
export class ImagePageComponent {
  selectedImage: picture | null = null;
  imageList: NgIterable<picture> = [];
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  selectedFile: File | null = null;

  constructor(private backendService: BackendServiceService) {
    this.fetchImages();
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
    var response = await this.backendService.uploadImage(formData)
    this.selectedImage = response;
    this.imageList = [...this.imageList, response];
  }

  private async fetchImages(): Promise<void> {
    // Fetch images from the backend and update imageList
    // This is a placeholder, implement the actual fetch logic
    this.imageList = await this.backendService.getImages();
  }
}

import {Component, ElementRef, NgIterable, ViewChild} from '@angular/core';
import {picture} from "../../models";

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

  constructor() {}

  triggerFileSelect(): void {
    this.fileInput.nativeElement.click();  // Opens the file picker
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.imageList.push(... this.imageList,
      this.uploadFile();
    }
  }

  uploadFile(): void {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.http.post('/api/upload', formData).subscribe({
      next: (res) => console.log('Upload successful', res),
      error: (err) => console.error('Upload failed', err)
    });
  }
}

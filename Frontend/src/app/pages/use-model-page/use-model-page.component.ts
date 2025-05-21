import { Component, OnInit } from '@angular/core';
import { TrainedModel } from 'src/app/models';
import { TrainedModelService } from 'src/app/services/trained-model.service';

@Component({
  selector: 'app-use-model-page',
  templateUrl: './use-model-page.component.html',
  styleUrls: ['./use-model-page.component.css']
})
export class UseModelPageComponent implements OnInit {
  uploadedImageUrl: string | null = null;
  imageFile: File | null = null;
  availableModels: TrainedModel[] = [];
  selectedModel: number | null = null;

  constructor(private trainedModelService: TrainedModelService) {}

  ngOnInit(): void {
    this.fetchModels();
  }

  async fetchModels() {
    try {
      this.availableModels = await this.trainedModelService.getTrainedModels();
    } catch (error) {
      console.error('Error fetching models:', error);
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();
      this.imageFile = file;
      reader.onload = () => {
        this.uploadedImageUrl = reader.result as string;
      };

      reader.readAsDataURL(file);
    }
  }

  async runModel() {
    if (!this.imageFile || !this.selectedModel) return;

    const formData = new FormData();
    formData.append('uploaded_file', this.imageFile);

    try {
      const imageBlob = await this.trainedModelService.useTrainedModel(this.selectedModel, formData);

      const reader = new FileReader();
      reader.onload = () => {
        this.uploadedImageUrl = reader.result as string;
      };
      reader.readAsDataURL(imageBlob);
    } catch (error) {
      console.error('Failed to run model:', error);
    }
  }
}

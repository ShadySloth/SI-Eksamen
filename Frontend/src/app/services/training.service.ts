import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment/environment';
import { Training } from '../models';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TrainingService {
  private baseUrl = environment.aiServiceBackend;
  constructor(private httpClient: HttpClient) { }

  async trainModel(training: Training) {
    const url = `${this.baseUrl}/api/ai/models/train`;
    try {
      const response = await firstValueFrom(this.httpClient.post(url, training));
      return response;
    } catch (error) {
      console.error('Error training model:', error);
      throw error;
    }
  }
}

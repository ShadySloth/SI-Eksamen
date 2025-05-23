import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { environment } from 'src/environment/environment';
import { TrainedModel } from '../models';

@Injectable({
  providedIn: 'root'
})

export class TrainedModelService {
  baseUrl: string = environment.aiServiceBackend;

  constructor(private httpClient: HttpClient) { }

  async getTrainedModels() {
    return await firstValueFrom(this.httpClient.get<TrainedModel[]>(this.baseUrl + "/api/ai/models"))
  }

  async useTrainedModel(idOfModel: number, image: FormData) {
    return await firstValueFrom(
      this.httpClient.post(this.baseUrl + "/api/ai/models/test/" + idOfModel, image, {
        responseType: 'blob' // 👈 important
      })
    );
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateDataSet, Dataset } from '../models';
import {firstValueFrom} from "rxjs";
import { environment } from 'src/environment/environment';

@Injectable({
  providedIn: 'root'
})
export class DatasetService {
  baseUrl: string = environment.backendService;

  constructor(private httpClient: HttpClient) { }

  async getDataset(datasetId: string) {
    return await firstValueFrom(
      this.httpClient.get<Dataset>(`${this.baseUrl}/data/${datasetId}`));
  }

  async getDatasets() {
    return await firstValueFrom(
      this.httpClient.get<Dataset[]>(this.baseUrl + "/data"));
  }

  async addDataset(dataset: CreateDataSet) {
    return await firstValueFrom(
      this.httpClient.post<Dataset>(this.baseUrl + "/data", dataset));
  }
}

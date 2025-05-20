import { Injectable } from '@angular/core';
import {environment} from "../../environment/environment.development";
import {HttpClient} from "@angular/common/http";
import {Segmentation} from "../models";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SegmentationService {
  baseUrl: string = environment.backendService;

  constructor(private httpClient: HttpClient) { }

  async addSegmentation(segmentation: Segmentation) {
    return await firstValueFrom(this.httpClient.post<Segmentation>(this.baseUrl + "/segmentation", segmentation))
  }

  async updateSegmentation(segmentation: Segmentation) {
    return await firstValueFrom(this.httpClient.put<Segmentation>(this.baseUrl + "/segmentation", segmentation))
  }

  async deleteSegmentation(segmentationId: string) {
    return await firstValueFrom(this.httpClient.delete<Segmentation>(this.baseUrl + "/segmentation/" + segmentationId))
  }

  async getSegmentations() {
    return await firstValueFrom(this.httpClient.get<Segmentation[]>(this.baseUrl + "/segmentation"))
  }

  async getSegmentationsByImageId(imageId: string) {
    return await firstValueFrom(this.httpClient.get<Segmentation[]>(this.baseUrl + "/segmentation/image/" + imageId))
  }
}

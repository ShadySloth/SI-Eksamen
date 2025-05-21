import { Injectable } from '@angular/core';
import {environment} from "../../environment/environment";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {Picture} from "../models";

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  baseUrl: string = environment.backendService;

  constructor(private httpClient: HttpClient) { }

  // function to upload an image to backend, should sent file to backend
  async uploadImage(image: FormData) {
    return await firstValueFrom(this.httpClient.post<Picture>(this.baseUrl + "/image", image))
  }

  async getImages() {
    return await firstValueFrom(this.httpClient.get<Picture[]>(this.baseUrl + "/image", ))
  }
}

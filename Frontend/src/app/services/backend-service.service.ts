import { Injectable } from '@angular/core';
import {environment} from "../../environment/environment";
import {HttpClient} from "@angular/common/http";
import {picture} from "../models";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class BackendServiceService {
  baseUrl: string = environment.backendService;

  constructor(private httpClient: HttpClient) { }

  // function to upload an image to backend, should sent file to backend
  async uploadImage(image: picture) {


    //return await firstValueFrom(this.httpClient.post<picture>(this.baseUrl + "/image", img))
  }
}

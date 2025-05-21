import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environment/environment.development";
import {firstValueFrom} from "rxjs";
import {Label} from "../models";

@Injectable({
  providedIn: 'root'
})

export class LabelService {
  baseUrl: string = environment.backendService;

  constructor(private httpClient: HttpClient) { }

  async getLabels() {
    return await firstValueFrom(this.httpClient.get<Label[]>(this.baseUrl + "/label"))
  }

  async addLabel(label: Label) {
    return await firstValueFrom(this.httpClient.post<Label>(this.baseUrl + "/label", label))
  }

  async updateLabel(label: Label) {
    var response = await firstValueFrom(this.httpClient.put<Label>(this.baseUrl + "/label", label))
  }
}

import { Component, OnInit } from '@angular/core';
import { CreateDataSet, Dataset, Label } from 'src/app/models';
import { DatasetService } from 'src/app/services/dataset.service';
import { LabelService } from 'src/app/services/label.service';

@Component({
  selector: 'app-dataset-page',
  templateUrl: './dataset-page.component.html',
  styleUrls: ['./dataset-page.component.css']
})

export class DatasetPageComponent implements OnInit {
  labels: Label[] = [];
  selectedLabels: string[] = [];
  trainingSetName: string = '';
  savedTrainingSets : Dataset[] = [];

  constructor(private datasetService: DatasetService,
              private labelService: LabelService) { }

  ngOnInit(): void {
    this.fetchLabels();
    this.fetchDatasets();
  }

  async fetchLabels() {
    var labels = await this.labelService.getLabels();
    if (labels) {
      this.labels = labels;
    }
    else {
      this.labels = [];
    }
  }

  async fetchDatasets() {
    var datasets =  await this.datasetService.getDatasets();
    if (datasets) {
      this.savedTrainingSets = datasets;
    }
    else {
      this.savedTrainingSets = [];
    }

    console.log("saved sets: ", this.savedTrainingSets);
  }

  selectLabel(label: Label) {
    const index = this.selectedLabels.findIndex(ids => ids === label.id);
    if (index === -1) {
      this.selectedLabels.push(label.id!);
    } else {
      this.selectedLabels.splice(index, 1);
    }
  }

  isSelected(label: Label): boolean {
    return this.selectedLabels.some(ids => ids === label.id);
  }

  async saveTrainingSet() {
    if (!this.trainingSetName.trim()) {
      alert('Please enter a name labels for the training set.');
      return;
    } else if (this.selectedLabels.length === 0) {
      alert('Please select at least one label for the training set.');
      return;
    }

    // TODO: Logic to save the training set
    console.log('Selected Labels:', this.selectedLabels);
    const newDataset: Dataset = {
      dataSetName: this.trainingSetName,
    }
    const newTrainingSet: CreateDataSet = {
      dataSet: newDataset,
      labelsToBeUsed: this.selectedLabels
    };

    var datasetReponse = await this.datasetService.addDataset(newTrainingSet);
    this.savedTrainingSets.push(datasetReponse!);
    this.selectedLabels = [];
    this.trainingSetName = '';
  }
}
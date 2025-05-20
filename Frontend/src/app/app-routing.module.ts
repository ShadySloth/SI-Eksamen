import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ImagePageComponent} from "./pages/image-page/image-page.component";
import {TrainingStatusComponent} from "./pages/training-status/training-status.component";

const routes: Routes = [
  {path : '', redirectTo: '/image', pathMatch: 'full'},
  {path : "image", component: ImagePageComponent},
  {path: "training", component: TrainingStatusComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

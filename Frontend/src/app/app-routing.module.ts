import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
/*
example routing:
const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home' }
];

add your routes in brackets below.
when adding new route all you really need is:
"{ path: 'about', component: AboutComponent },"
 */
const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

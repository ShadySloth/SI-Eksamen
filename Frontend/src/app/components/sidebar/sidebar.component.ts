import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {

  // redirect to a given URL, eg. google.dk
  easterComesEarly() {
    window.location.href = 'https://youtu.be/IqPr6bcTJhc?si=l6HLynJS0ow9Q8cD&t=10';
  }
}

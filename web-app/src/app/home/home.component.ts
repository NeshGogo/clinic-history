import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from '../shared/components/nav/nav.component';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NavComponent],
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(titleService: Title){
    titleService.setTitle('NC | Home');
  }
}

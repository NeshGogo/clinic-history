import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-speciality',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './speciality.component.html',
})
export class SpecialityComponent {
  title = 'Specialities';
}

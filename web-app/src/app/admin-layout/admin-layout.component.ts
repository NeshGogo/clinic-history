import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from '../shared/components/nav/nav.component';
import { SidebarComponent } from '../shared/components/sidebar/sidebar.component';
import { LinkItem } from '../core/models/linkItem';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, NavComponent, SidebarComponent],
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.css']
})
export class AdminLayoutComponent {
  menuItems: LinkItem[] = [
    {
      name:'Doctors',
      path:'doctors',
      iconUrl: 'https://img.icons8.com/external-flatart-icons-lineal-color-flatarticons/64/external-doctors-biochemistry-and-medicine-healthcare-flatart-icons-lineal-color-flatarticons.png',
      iconAlt: 'Doctors icon',
    },
    {
      name:'Specialties',
      path:'specialties',
      iconUrl: 'https://img.icons8.com/color/48/groups.png',
      iconAlt: 'specialties icon',
    }
  ]
}

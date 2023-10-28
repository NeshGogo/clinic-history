import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from '../../shared/components/nav/nav.component';
import { SidebarComponent } from '../../shared/components/sidebar/sidebar.component';
import { LinkItem } from '../../core/models/linkItem';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, NavComponent, SidebarComponent, RouterOutlet],
  templateUrl: './layout.component.html',
})
export class LayoutComponent {
  show = false;
  menuItems: LinkItem[] = [
    {
      name:'Doctors',
      path:'doctors',
      iconUrl: 'https://img.icons8.com/external-flatart-icons-lineal-color-flatarticons/64/external-doctors-biochemistry-and-medicine-healthcare-flatart-icons-lineal-color-flatarticons.png',
      iconAlt: 'Doctors icon',
    },
    {
      name:'Specialities',
      path:'specialities',
      iconUrl: 'https://img.icons8.com/color/48/groups.png',
      iconAlt: 'specialities icon',
    }
  ]

  showMenu(){
    this.show = true;
  }
}

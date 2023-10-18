import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClickOutsideDirective } from '../../directives/click-outside.directive';
import { AuthService } from 'src/app/core/services/auth.service';
import { User } from 'src/app/core/models/user';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule, ClickOutsideDirective],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  avatarUrl: string = 'https://images.rawpixel.com/image_png_800/cHJpdmF0ZS9sci9pbWFnZXMvd2Vic2l0ZS8yMDIyLTA0L3BmLWljb240LWppcjIwNjItcG9yLWwtam9iNzg4LnBuZw.png';
  showMenu: boolean = false;
  user: User | null = null;

  constructor(private authService: AuthService){}
 
  ngOnInit(): void {
    this.authService.user$
    .subscribe(user => {
      this.user = user;
    });
  }

  ShowUserMenu(ev: Event){
    ev.preventDefault();
    this.showMenu = !this.showMenu;
  }

  clickedOutside(): void {
    this.showMenu = false;
  }
}

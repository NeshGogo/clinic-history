import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClickOutsideDirective } from '../../directives/click-outside.directive';
import { AuthService } from 'src/app/core/services/auth.service';
import { User } from 'src/app/core/models/user';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule, ClickOutsideDirective, RouterModule],
  templateUrl: './nav.component.html',
})
export class NavComponent implements OnInit{
  avatarUrl: string = 'https://0.gravatar.com/avatar/2df5c64b6e9ec9308b1dec129bb88fb773cf573cc93418507e14b7d241cdee74?size=128';
  showMenu: boolean = false;
  user: User | null = null;
  @Output() showSideBarMenu:  EventEmitter<void> = new EventEmitter();

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

  showSidebar(): void {
    this.showSideBarMenu.emit();
  }
}

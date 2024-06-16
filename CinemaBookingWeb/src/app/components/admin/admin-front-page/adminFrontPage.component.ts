import { Component } from '@angular/core';
import { Movie } from 'src/app/models/movie/movie';
import { VisibilityService } from 'src/app/services/visibility.service';

@Component({
  selector: 'appadminfrontpage',
  templateUrl: './adminFrontpage.component.html',
  styleUrls: ['./adminFrontPage.component.css']
})
// Export a component class responsible for handling front page administrative functionalities.
export class AdminFrontPageComponent {
  // State to track the visibility of the menu on the admin front page.
  menuVisible: boolean = false;

  // Constructor which injects the VisibilityService for managing section visibilities.
  constructor(public visibilityService: VisibilityService) {}

  // Method to toggle the state of the menu visibility.
  toggleMenu(): void {
    console.log("Menu toggled");
    this.menuVisible = !this.menuVisible;
  }
  
  // Method to show a particular section by first resetting all sections' visibilities, then enabling visibility for the specified section.
  showSection(section: string): void {
    this.visibilityService.resetVisibility();
    this.visibilityService.toggleVisibility(section);
  }
}

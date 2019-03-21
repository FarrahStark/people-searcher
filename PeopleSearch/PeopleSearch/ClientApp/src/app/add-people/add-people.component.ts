import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-add-people',
  templateUrl: './add-people.component.html'
})
export class AddPeopleComponent {
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
  }
}

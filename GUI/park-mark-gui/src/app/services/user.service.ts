import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { HttpClientModule} from '@angular/common/http';
import { config } from 'rxjs';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  url="https://localhost:44374/api/users"
  constructor(private http:HttpClient) { }

  
  SignUp(newUser:User):Observable<number>
  {
  //let  headers=new Headers({'Content-type':'application/json; charset=utf-8'});
  return this.http.post<number>(`${this.url}/signUp`,newUser);
  }

}
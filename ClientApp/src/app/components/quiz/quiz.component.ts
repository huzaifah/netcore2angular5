import { Component, Input, Inject } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "quiz",
  templateUrl: './quiz.component.html',
  styleUrls: ['./quiz.component.css']
})

export class QuizComponent {
  quiz: Quiz;

  constructor(private activatedRoute: ActivatedRoute,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router) {

    // create an empty object from the Quiz interface
    this.quiz = <Quiz>{};

    var id = +this.activatedRoute.snapshot.params["id"];
    console.log(id);

    if (id) {
      // TODO: load the quiz using server-side API
      var url = this.baseUrl + "api/quiz/" + id;

      this.http.get<Quiz>(url).subscribe(result => {
        this.quiz = result;
      }, error => console.error(error));

    } else {
      console.log("Invalid id: routing back to home...");
      this.router.navigate(["home"]);
    }
  }

  onEdit() {
    this.router.navigate(["quiz/edit", this.quiz.Id]);
  }

  onDelete() {
    if (confirm("Do you really want to delete this quiz?")) {
      var url = this.baseUrl + "api/quiz/" + this.quiz.Id;
      this.http
        .delete(url)
        .subscribe(res => {
          console.log("Quiz " + this.quiz.Id + " has been deleted.");
          this.router.navigate(["home"]);
        }, error => console.log(error));
    }
  }
}
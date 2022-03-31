<?php ?>
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>LDAP-System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3" crossorigin="anonymous">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js" integrity="sha256-/xUj+3OJU5yExlq6GSYGSHk7tPXikynS7ogEvDej/m4=" crossorigin="anonymous"></script>
    <script src="./logic.js"></script>

</head>

<body class="container">

    <div class="ldap-form pt-2">
<h2>Benutzer hinzufügen editieren</h1>
<hr>
    <form onsubmit="event.preventDefault();onFormSubmit();" autocomplete="off">
  <div class="form-group">
    <label for="exampleInputEmail1">Cn</label>
    <input class="form-control" required id="cn"  placeholder="Enter CN">
  </div>
  <div class="form-group">
    <label for="exampleInputPassword1">Sn</label>
    <input class="form-control" id="sn" placeholder="Enter SN">
  </div>
  <div class="form-group">
    <label for="exampleInputPassword1">Password</label>
    <input class="form-control" id="userPassword" placeholder="Enter Password">
  </div>
 <div class="pt-2">
  <button type="submit" id="submitButton" class="btn btn-primary">Hinzufügen</button>
  <button type="button" onClick="resetForm()" class="btn btn-danger">Reset</button>
 </div>
</form>
    </div>
    <br />
    <h2>Ldap-Benutzer</h1>
<hr>
    <div class="ldap-table">
        <table class="table" class="list" id="ldapList">
            <thead >
                <tr>
                    <th scope="col">CN</th>
                    <th scope="col">SN</th>
                    <th scope="col">Password</th>
                    <th scope="col">Aktionen</th>
                </tr>
            </thead>
            <tbody>

            </tbody>
        </table>
    </div>
</body>

</html>
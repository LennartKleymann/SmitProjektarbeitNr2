<?php ?>

<h1>Users</h1>

<table>
    <tr>
        <th>CN</th>
        <th>SN</th>
    </tr>
<?php
$arrContextOptions=array(
    "ssl"=>array(
        "verify_peer"=>false,
        "verify_peer_name"=>false,
    ),
);

$users = file_get_contents("http://localhost:5207/User", false, stream_context_create($arrContextOptions));
$json_string = json_decode($users, JSON_PRETTY_PRINT);

foreach ($json_string as $item){
    echo "<tr>";
    echo "<td>" . $item["cn"]. "</td>";
    echo "<td>" . $item["sn"]. "</td>";
    echo "</tr>";
}

?>
</table>

<h1>Benutzer hinzufügen</h1>
<form action="https://localhost:7207/User" method="post">
    <label for="cn">Cn:</label><br>
    <input type="text" id="cn" name="cn" value="test"><br>
    <label for="sn">Lastname:</label><br>
    <input type="text" id="sn" name="sn" value="Doe"><br><br>
    <input type="submit" value="Submit">
</form>

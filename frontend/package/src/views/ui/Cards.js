import {
  Card,
  CardImg,
  CardText,
  CardBody,
  CardTitle,
  CardSubtitle,
  CardGroup,
  Button,
  Row,
  Col, Form, FormGroup, Label, Input, Table,
} from "reactstrap";
import Blog from "../../components/dashboard/Blog";
import bg1 from "../../assets/images/bg/bg1.jpg";
import bg2 from "../../assets/images/bg/bg2.jpg";
import bg3 from "../../assets/images/bg/bg3.jpg";
import bg4 from "../../assets/images/bg/bg4.jpg";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPrescriptionBottle} from "@fortawesome/free-solid-svg-icons";
import React, {useEffect, useState} from "react";
import axios from "axios";
import ProjectTables from "../../components/dashboard/PharmaciensTables";


const Cards = () => {
  const [pharmaciens, setPharmaciens] = useState([]);
  const [pharmacien, setPharmacien] = useState({
    nom: "",
    prenom: "",
    mail: "",

  });


  // Fonction pour récupérer les Pharmaciens


  const fetchPharmaciens = async () => {
    try {
      const res = await axios.get("http://localhost:5156/api/Pharmacien");
      setPharmaciens(res.data);
      console.log(res.data);
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    fetchPharmaciens();
  }, []);

  const handleSave = async (e) => {
    try {
      e.preventDefault();

      console.log("Données du Pharmacien :", pharmacien);

      await axios.post("http://localhost:5156/api/Pharmacien", pharmacien)
          .then((res) => {
            setPharmacien({
              nom: "",
              prenom: "",
              email: "",
            });

            fetchPharmaciens();

          });
    } catch (error) {
      console.error("Erreur lors de la creation d'un Pharmaciens:", error);
    }
  };










  return (
    <div>
      <Row>
        <Col>
          <Card>
            <CardTitle tag="h6" className="border-bottom p-3 mb-0">
              <FontAwesomeIcon icon={faPrescriptionBottle} style={{marginRight:20}} />
              Gestion des pharmaciens
            </CardTitle>
            <CardBody>
              <Form>
                <div>
                  <FormGroup >
                    <Label for="exampleEmail">Nom</Label>
                    <Input
                        id="name"
                        type="string"
                        name="nom"
                        placeholder="nom"
                        value={pharmacien.nom}
                        onChange={(e) => setPharmacien ({...pharmacien,nom:e.target.value})}

                    />
                  </FormGroup>
                  <FormGroup >
                    <Label for="exampleEmail">Prénom</Label>
                    <Input
                        id="prenom"
                        type="string"
                        name="prenom"
                        placeholder="prenom"
                        value={pharmacien.prenom}
                        onChange={(e) => setPharmacien ({...pharmacien,prenom:e.target.value})}


                    />
                  </FormGroup>
                  <FormGroup >
                    <Label for="exampleEmail">Email</Label>
                    <Input
                        id="email"
                        type="email"
                        name="email"
                        placeholder="email"
                        value={pharmacien.email}
                        onChange={(e) => setPharmacien ({...pharmacien,email:e.target.value})}


                    />
                  </FormGroup>



                </div>
                <Button onClick={(e)=>handleSave(e)}>Ajouter</Button>

              </Form>
            </CardBody>
          </Card>
        </Col>
      </Row>
      <Row className="mt-4">
        {/* <Col>
          <Card>
            <CardBody>
              <Table responsive>
                <thead>
                <tr>
                  <th>#</th>
                  <th>Nom</th>
                  <th>Prénom</th>
                  <th>Email</th>

                </tr>
                </thead>
                <tbody> */}
                {/*{patients.map((patient, index) => (*/}
                {/*    <Col key={index}>*/}
                {/*      <tr key={patient.id}>*/}
                {/*        <td>{index + 1}</td>*/}
                {/*        <td>{patient.nom}</td>*/}
                {/*        <td>{patient.prenom}</td>*/}
                {/*        <td>{patient.email}</td>*/}

                {/*        <td>*/}
                {/*          <Button color="warning" size="sm" >*/}
                {/*            <FontAwesomeIcon icon={faEdit}/>*/}
                {/*          </Button>{" "}*/}
                {/*          <Button color="danger" size="sm" >*/}
                {/*            <FontAwesomeIcon icon={faTrashAlt}/>*/}
                {/*          </Button>*/}
                {/*        </td>*/}
                {/*      </tr>*/}
                {/*    </Col>*/}
                {/*))}*/}


                {/* </tbody>
              </Table>
            </CardBody>
          </Card>
        </Col> */}

        <Col lg="12">
        <ProjectTables pharmaciens={pharmaciens} pharmacien={pharmacien} setPharmacien={setPharmacien} setPharmaciens={setPharmaciens}/>
        </Col>
      </Row>



    </div>
  );
};
export default Cards
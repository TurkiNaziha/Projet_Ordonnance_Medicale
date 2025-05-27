import React, {useEffect, useState} from "react";
import {
  Button,
  ButtonGroup,
  Card,
  CardBody,
  CardTitle,
  Row,
  Col, Form, Label, Input, FormGroup,
} from "reactstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUserInjured, faUserMd} from "@fortawesome/free-solid-svg-icons";
import axios from "axios";
import ProjectTables from "../../components/dashboard/PatientsTables";

const Buttons = () => {

  const [medecins, setMedecins] = useState([]);
  const [selectedMedecin, setSelectedMedecin] = useState({medecinID:"", medecinName:""});
  // Fonction pour récupérer les médecins
  const fetchMedecins = async () => {
    try {
      const response = await axios.get("http://localhost:5156/api/Medecin"); // Remplacez par votre URL API
      setMedecins(response.data);
    } catch (error) {
      console.error("Erreur lors de la récupération des médecins :", error);
    }
  };

  useEffect(() => {
    fetchMedecins();
  }, []);

  const handleMedecinChange = (event) => {
    const selectedID = event.target.value; // Get selected medecin ID
    const medecin = medecins.find((m) => m.id === parseInt(selectedID)); // Find the medecin object

    if (medecin) {
      setSelectedMedecin({
        medecinID: medecin.id,
        medecinName: `${medecin.nom} ${medecin.prenom}`,
      });
    } else {
      // Reset the state if no medecin is selected
      setSelectedMedecin({
        medecinID: "",
        medecinName: "",
      });
    }
  };

  const [patients, setPatients] = useState([]);
  const [patient, setPatient] = useState([]);
  const fetchspatients=async()=>{
    try {
      const res=await axios.get("http://localhost:5156/api/Patient")
      setPatients(res.data )
      console.log(res.data )
      // setisLoading(false)
    } catch (error) {
      console.log(error)
    }

  }

  useEffect(()=>{
    fetchspatients()
  },[])



  const handleSave = async (e) => {
    try {
      e.preventDefault();
      const patientData = { 
        ...patient, 
        medecinID: selectedMedecin.medecinID, 
        medecinName: selectedMedecin.medecinName 
      };
      
      console.log("Données du patient :", patientData);

      await axios.post("http://localhost:5156/api/Patient", patientData)
          .then((res) => {
            setPatient({
              nom: "",
              prenom: "",
              mail: "",
              telephone: "",
              medecinID: "",
              maladie:""
            });
            setSelectedMedecin("");
            fetchspatients();

          });
    } catch (error) {
      console.error("Erreur lors de la creation d'un patient:", error);
    }
  };
  return (
    <div>
      <Row>
        <Col>
          <Card>
            <CardTitle tag="h6" className="border-bottom p-3 mb-0">
              <FontAwesomeIcon icon={faUserInjured} style={{marginRight:20}} />
              Gestion des patients
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
                        value={patient.nom}
                        onChange={(e) => setPatient ({...patient,nom:e.target.value})}

                    />
                    </FormGroup>
                    <FormGroup >
                    <Label for="exampleEmail">Prénom</Label>
                    <Input
                        id="prenom"
                        type="string"
                        name="prenom"
                        placeholder="prenom"
                        value={patient.prenom}
                        onChange={(e) => setPatient ({...patient,prenom:e.target.value})}


                    />
                    </FormGroup>
                    <FormGroup >
                    <Label for="exampleEmail">Email</Label>
                    <Input
                        id="mail"
                        type="email"
                        name="email"
                        placeholder="email"
                        value={patient.mail}
                        onChange={(e) => setPatient ({...patient,mail:e.target.value})}


                    />
                    </FormGroup>
                    <FormGroup >
                    <Label for="exampleEmail">Numéro de Telephone</Label>
                    <Input
                        id="num"
                        type="string"
                        name="telephone"
                        placeholder="telephone"
                        value={patient.telephone}
                        onChange={(e) => setPatient ({...patient,telephone:e.target.value})}


                    />

                  </FormGroup>
                  <FormGroup>
                    <Label for="exampleEmail">Médecin</Label>
                    <Input
                        id="medecinID"
                        type="select"
                        name="medecinID"
                        value={selectedMedecin.medecinID} // Lier l'état
                        onChange={handleMedecinChange} // Gestionnaire d'événements
                    >
                      <option value="">-- Sélectionner votre médecin --</option>
                      {medecins.map((medecin) => (
                          <option key={medecin.id} value={medecin.id}>
                            {medecin.nom} {medecin.prenom} - {medecin.specialite}
                          </option>
                      ))}
                    </Input>
                  </FormGroup>
                  <FormGroup >
                    <Label for="exampleEmail">Maladie</Label>
                    <Input
                        id="maladie"
                        type="string"
                        name="maladie"
                        placeholder="maladie"
                        value={patient.maladie}
                        onChange={(e) => setPatient({...patient, maladie: e.target.value})}

                    />
                  </FormGroup>
                </div>
                <Button onClick={(e)=>handleSave(e)}>Ajouter</Button>

              </Form>
            </CardBody>
          </Card>
        </Col>

        <Col lg="12">
        <ProjectTables patients={patients} patient={patient} setPatient={setPatient} setPatients={setPatients}/>
        </Col>
      </Row>







      {/*    <Card>*/}
      {/*      <CardTitle tag="h6" className="border-bottom p-3 mb-0">*/}
      {/*        Checkbox(Stateful Buttons)*/}
      {/*      </CardTitle>*/}
      {/*      <CardBody className="">*/}
      {/*        <h5>Checkbox Buttons</h5>*/}
      {/*        <ButtonGroup>*/}
      {/*          <Button*/}
      {/*            color="primary"*/}
      {/*            onClick={() => onCheckboxBtnClick(1)}*/}
      {/*            active={cSelected.includes(1)}*/}
      {/*          >*/}
      {/*            One*/}
      {/*          </Button>*/}
      {/*          <Button*/}
      {/*            color="primary"*/}
      {/*            onClick={() => onCheckboxBtnClick(2)}*/}
      {/*            active={cSelected.includes(2)}*/}
      {/*          >*/}
      {/*            Two*/}
      {/*          </Button>*/}
      {/*          <Button*/}
      {/*            color="primary"*/}
      {/*            onClick={() => onCheckboxBtnClick(3)}*/}
      {/*            active={cSelected.includes(3)}*/}
      {/*          >*/}
      {/*            Three*/}
      {/*          </Button>*/}
      {/*        </ButtonGroup>*/}
      {/*        <p className="mb-0">Selected: {JSON.stringify(cSelected)}</p>*/}
      {/*      </CardBody>*/}
      {/*    </Card>*/}
      {/*  </Col>*/}
      {/*  <Col xs="12" md="6">*/}
      {/*    /!* --------------------------------------------------------------------------------*!/*/}
      {/*    /!* Card-6*!/*/}
      {/*    /!* --------------------------------------------------------------------------------*!/*/}
      {/*    <Card>*/}
      {/*      <CardTitle tag="h6" className="border-bottom p-3 mb-0">*/}
      {/*        Radio Buttons (Stateful Buttons)*/}
      {/*      </CardTitle>*/}
      {/*      <CardBody className="">*/}
      {/*        <h5>Radio Buttons</h5>*/}
      {/*        <ButtonGroup>*/}
      {/*          <Button*/}
      {/*            color="primary"*/}
      {/*            onClick={() => onRadioBtnClick(1)}*/}
      {/*            active={rSelected === 1}*/}
      {/*          >*/}
      {/*            One*/}
      {/*          </Button>*/}
      {/*          <Button*/}
      {/*            color="primary"*/}
      {/*            onClick={() => onRadioBtnClick(2)}*/}
      {/*            active={rSelected === 2}*/}
      {/*          >*/}
      {/*            Two*/}
      {/*          </Button>*/}
      {/*          <Button*/}
      {/*            color="primary"*/}
      {/*            onClick={() => onRadioBtnClick(3)}*/}
      {/*            active={rSelected === 3}*/}
      {/*          >*/}
      {/*            Three*/}
      {/*          </Button>*/}
      {/*        </ButtonGroup>*/}
      {/*        <p className="mb-0">Selected: {rSelected}</p>*/}
      {/*      </CardBody>*/}
      {/*    </Card>*/}
      {/*  </Col>*/}
      {/*</Row>*/}
      {/*/!* --------------------------------------------------------------------------------*!/*/}
      {/*/!* Row*!/*/}
      {/*/!* --------------------------------------------------------------------------------*!/*/}

      {/*/!* --------------------------------------------------------------------------------*!/*/}
      {/*/!* End Inner Div*!/*/}
      {/*/!* --------------------------------------------------------------------------------*!/*/}
    </div>
  );
};

export default Buttons;
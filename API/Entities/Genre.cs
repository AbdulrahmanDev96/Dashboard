using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Genre
    {
        // lw men el bdaya 3mltow int kan 3mlow fe database identity column
        // fa ybda2 yzowed men 1, 2, 3, .... m3a kol insert
        // 3l4an myb24 mstny any 2b3t el id bta3 el genre we 3ayzow ydef el number da men nfsow
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public byte Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
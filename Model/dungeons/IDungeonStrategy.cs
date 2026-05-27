using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    public interface IDungeonGenerationStrategy
    {
        void Generate(IBuilder builder);
    }
}
